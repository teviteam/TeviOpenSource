<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$jsondata = $_POST["myform_soils"]; 

$myObj2 = new stdClass();
$myObj2 =json_decode($jsondata, TRUE);

$okToSaveSoil =false;
$player_id=0;

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
					$okToSaveSoil=true;						
				}
				else 
				{
					echo "No player with this id in the DBB";	
					$okToSaveSoil=false;	
				}
			}
		}
	}
}

if($okToSaveSoil==true)   
{
	//updating soils
	foreach ($myObj2["Items"] as $items) {
		$idSoil=0;
		$sqlclause="";
		$sql2=array(); //important to empty the request and avoid having two items at a time.
		
		//parsing content of each soil
		foreach ($items as $key => $val) {
			//verifying if it is a new user or  not
			if($key=="id_soil")
			{
				if ($val!=0)
				{
					$idSoil=$val;
				}
			}			
			$sql2[] = (is_numeric($val)) ? "`$key` = $val" : "`$key` = '" . mysqli_real_escape_string($database,$val) . "'";
		}
		
		//updating soil
		$sqlclause = implode(",",$sql2);
		//echo $sqlclause." --- " ;
		$rs = mysqli_query($database,"UPDATE `kr_soils` SET $sqlclause WHERE id_soil = ".$idSoil) or die("DATABASE ERROR SAVING SOILS!");
	}
	
	echo "save done";
}

// Close mySQL Connection
mysqli_close($database);
?>