<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$playerid = anti_injection_login($_POST["myform_playerid"]); 

$okToLoadPlantnetResults =false;

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
			$okToLoadPlantnetResults=true;						
		}
		else 
		{
			echo "No player with this id in the DBB";	
			$okToLoadPlantnetResults=false;	
		}		
	}
}

if($okToLoadPlantnetResults==true)   
{
	//getting plantnet result data for the user
	$SQL3 = "SELECT * FROM kr_plantnet WHERE player_id = '" . $playerid . "'";
	$results = @mysqli_query($database,$SQL3) or die("DATABASE ERROR SELECTING PLANTNET RESULTS!");
	$total3 = mysqli_num_rows($results);
	$myObjArray=array();
	
	if($total3) 
	{		
		while($row = mysqli_fetch_assoc($results)){
			//echo "row id plant : ".$row["id_plant"]." -- ";
			
			//json conversion of the array					
			$myObj = new stdClass();			
		
			$myObj->id_plant = $row["id_plant"]; 
			$myObj->player_id = $playerid;
			$myObj->image_name = $row["image_name"]; 
			$myObj->plant0 = $row["plant0"]; 
			$myObj->common0 = $row["common0"]; 
			$myObj->score0 = $row["score0"]; 
			$myObj->plant1 = $row["plant1"]; 
			$myObj->common1 = $row["common1"]; 
			$myObj->score1 = $row["score1"]; 
			$myObj->plant2 = $row["plant2"]; 
			$myObj->common2 = $row["common2"]; 
			$myObj->score2 = $row["score2"]; 
			$myObj->date = $row["date"]; 

			$myObjArray[] = $myObj;

		}
		$myJSON = json_encode($myObjArray);
		echo $myJSON;
	}
	else {
		echo "No plantnet results";
	}
}

// Close mySQL Connection
mysqli_close($database);
?>