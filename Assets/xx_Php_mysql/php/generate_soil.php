<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$jsondata = $_POST["myform_soils"]; 

$myObj2 = new stdClass();
$myObj2 =json_decode($jsondata, TRUE);

$okToRegister =false;
$player_id=0;

$newSoil=true;
//echo $myObj;
foreach ($myObj2["Items"] as $items) {
	foreach ($items as $key => $val) {
		//echo $key;
		if ($unityHash != $phpHash)
		{
			echo "HASH code is diferent from your game, you infidel.";
		} 
		else
		{
			if($key=="player_id")
			{
				//echo $val;
				$SQL = "SELECT * FROM kr_users WHERE playerId = '" . $val . "'";
				$result_id = @mysqli_query($database,$SQL) or die("DATABASE ERROR!");
				$total = mysqli_num_rows($result_id);
				if($total) 
				{
					$player_id=$val;	
					$okToRegisterSoil=true;						
				}
				else 
				{
					echo "No player with this id in the DBB";	
					$okToRegisterSoil=false;	
				}
				
				//check if there are some soils for our player in the soils database already
				$SQL5 = "SELECT * FROM kr_soils WHERE player_id = '" . $val . "'";
				$result_id5 = @mysqli_query($database,$SQL5) or die("DATABASE ERROR!");
				$total5 = mysqli_num_rows($result_id5);
				if($total5) 
				{
					$newSoil=false;			
				}
				else 
				{
					//echo "No player with this id in the DBB";	
					$newSoil=true;
				}
			}
		}
	}
}

if($okToRegisterSoil==true)   
{
	//creating new soils in the table if the user is new
	foreach ($myObj2["Items"] as $items) {
		$sqlclause="";
		$sql2=array(); //important to empty the request and avoid having two items at a time.
		
		//getting unique id of each soil //not working
		//$rs2 = mysql_query("SELECT auto_Increment FROM INFORMATION_SCHEMA.tables WHERE table_name='kr_soils'") or die("DATABASE ERROR GETTING LAST ID!");
		//echo "soils inserted :".$rs2;
		
		//parsing content of each soil
		foreach ($items as $key => $val) {
			/*
			//verifying if it is a new user or  not
			if($key=="id_soil")
			{
				if ($val!=0)
				{
					$newSoil=false;
				}
			}	*/		
			$sql2[] = (is_numeric($val)) ? "`$key` = $val" : "`$key` = '" . mysqli_real_escape_string($database,$val) . "'";
		}
		
		if($newSoil==true)
		{
			//inserting soil
			$sqlclause = implode(",",$sql2);
			//echo $sqlclause." --- " ;
			$rs = mysqli_query($database,"INSERT INTO `kr_soils` SET $sqlclause") or die("DATABASE ERROR REGISTERING SOILS!");
			
			//echo "INSERT INTO `kr_soils` SET $sqlclause";
		}
	}
	
	//getting soil data for the user
	
	$SQL3 = "SELECT * FROM kr_soils WHERE player_id = '" . $player_id . "'";
	$result_id3 = @mysqli_query($database,$SQL3) or die("DATABASE ERROR!");
	$total3 = mysqli_num_rows($result_id3);
	$myObjArray=array();
	
	if($total3) 
	{		
		while($row = mysqli_fetch_assoc($result_id3)){
			//echo "row id soil : ".$row["id_soil"]." -- ";
			
			//json conversion of the array					
			$myObj = new stdClass();
			
			$myObj->id_soil = $row["id_soil"];
			$myObj->player_id = $player_id;
			$myObj->soil_name = $row["soil_name"];
			$myObj->posX = $row["posX"];
			$myObj->posY = $row["posY"];
			$myObj->water_lvl = $row["water_lvl"];
			$myObj->nutrient_lvl = $row["nutrient_lvl"];
			$myObj->is_planted = $row["is_planted"];
			$myObj->has_babyplant = $row["has_babyplant"];
			$myObj->plant_name = $row["plant_name"];
			$myObj->plant_time = $row["plant_time"];
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