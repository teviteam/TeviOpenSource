<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$playerid = anti_injection_login($_POST["myform_playerid"]); 

$okToRegister =false;

$newInventory=true;

if ($unityHash != $phpHash)
{
	echo "HASH code is diferent from your game, you infidel.";
} 
else
{
	if($playerid!=0)
	{
		//echo $val;
		$SQL = "SELECT * FROM kr_users WHERE playerId = '" . $playerid . "'";
		$result_id = @mysqli_query($database,$SQL) or die("DATABASE ERROR!");
		$total = mysqli_num_rows($result_id);
		if($total) 
		{
			$okToRegisterSoil=true;						
		}
		else 
		{
			echo "No player with this id in the DBB";	
			$okToRegisterSoil=false;	
		}
		
		//check if there are some inventory for our player in the inventory table already
		$SQL5 = "SELECT * FROM kr_inventory WHERE player_id = '" . $playerid . "'";
		$result_id5 = @mysqli_query($database,$SQL5) or die("DATABASE ERROR!");
		$total5 = mysqli_num_rows($result_id5);
		if($total5) 
		{
			$newInventory=false;			
		}
		else 
		{
			$newInventory=true;
		}
	}
}


if($okToRegisterSoil==true)   
{
	//creating new inventory in the table if the user is new		
	if($newInventory==true)
	{
		//inserting inventory
		$rs = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','Inspector','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		$rs1 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','WateringCan','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		$rs2 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','Fertilizer','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		
		$rs3 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','BananaSeed','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		$rs4 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','BreadfruitSeed','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		$rs5 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','ShampooGingerSeed','1') ") or die("DATABASE ERROR CREATING INVENTORY!");
		$rs6 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','SugarcaneSeed','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		$rs7 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','WildindigoSeed','0') ") or die("DATABASE ERROR CREATING INVENTORY!");

		$rs8 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','BananaFruit','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		$rs9 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','BreadfruitFruit','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		$rs10 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','ShampooGingerFruit','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		$rs11 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','SugarcaneFruit','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		$rs12 = mysqli_query($database,"INSERT INTO `kr_inventory` (id_inventory,player_id,itemName,itemCount) VALUES ('','" . $playerid . "','WildindigoFruit','0') ") or die("DATABASE ERROR CREATING INVENTORY!");
		
	}

	//getting inventory data for the user
	$SQL3 = "SELECT * FROM kr_inventory WHERE player_id = '" . $playerid . "'";
	$result_id3 = @mysqli_query($database,$SQL3) or die("DATABASE ERROR!");
	$total3 = mysqli_num_rows($result_id3);
	$myObjArray=array();
	
	if($total3) 
	{		
		while($row = mysqli_fetch_assoc($result_id3)){
			//echo "row id soil : ".$row["id_soil"]." -- ";
			
			//json conversion of the array					
			$myObj = new stdClass();
			
			$myObj->id_inventory = $row["id_inventory"];
			$myObj->player_id = $playerid;
			$myObj->itemName = $row["itemName"];
			$myObj->itemCount = $row["itemCount"];
			$myObjArray[] = $myObj;

		}
		$myJSON = json_encode($myObjArray);
		echo $myJSON;
	}
	else {
		echo "Data invalid - cant find any soils for player_id.";
	}
}

// Close mySQL Connection
mysqli_close($database);
?>