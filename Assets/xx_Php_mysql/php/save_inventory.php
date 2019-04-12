<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$jsondata = $_POST["myform_inventory"]; 

$myObj2 = new stdClass();
$myObj2 =json_decode($jsondata, TRUE);

$okToSaveInventory =false;
$player_id=0;

foreach ($myObj2["Items"] as $items) {
	foreach ($items as $key => $val) {
		//echo "key:".$key;
		if ($unityHash != $phpHash)
		{
			echo "HASH code is diferent from your game, you infidel.";
		} 
		else
		{
			if($key=="player_id")
			{
				//echo "val:".$val;
				$SQL = "SELECT * FROM kr_users WHERE playerId = '" . $val . "'";
				$result_id = @mysqli_query($database,$SQL) or die("DATABASE ERROR!");
				$total = mysqli_num_rows($result_id);
				if($total) 
				{
					$player_id=$val;	
					$okToSaveInventory=true;						
				}
				else 
				{
					echo "No player with this id in the DBB". $val;	
					$okToSaveInventory=false;	
				}
			}
		}
	}
}

if($okToSaveInventory==true)   
{
	//creating new inventory in the table if the object is new or updating if it is not
	foreach ($myObj2["Items"] as $items) {
		$idInventory=0;
		$sqlclause="";
		$sql2=array(); //important to empty the request and avoid having two items at a time.*
		$sql3=array();
		$updateInventory=false;	
		$insertInventory=false;			
		
		//parsing content of each soil
		foreach ($items as $key => $val) {
			//verifying if it is a new user or  not
			if($key=="id_inventory")
			{
				if ($val!=0)
				{
					$idInventory=$val;
				}
				
				//echo $val;
				$SQL = "SELECT * FROM kr_inventory WHERE id_inventory = '" . $val . "'";
				$result_id = @mysqli_query($database,$SQL) or die("DATABASE ERROR!");
				$total = mysqli_num_rows($result_id);
				if($total) 
				{
					//echo "inventory object is in the database => updating";	
					$updateInventory=true;						
				}

			}			
			$sql2[] = (is_numeric($val)) ? "`$key` = $val" : "`$key` = '" . mysqli_real_escape_string($database,$val) . "'";
		}
		
		//actually updating 
		if($updateInventory==true)
		{
			$sqlclauseupdate = implode(",",$sql2);
			//echo $sqlclauseupdate." --- " ;
			$rs = mysqli_query($database,"UPDATE `kr_inventory` SET $sqlclauseupdate WHERE id_inventory = ".$idInventory) or die("DATABASE ERROR SAVING INVENTORY UPDATE!");
		}		
	}	
	echo "inventory done";
}

// Close mySQL Connection
mysqli_close($database);
?>