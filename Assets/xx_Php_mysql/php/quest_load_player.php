<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$playerid = anti_injection_login($_POST["myform_playerid"]); 

$okToLoadPlayerQuest =false;

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
			$okToLoadPlayerQuest=true;						
		}
		else 
		{
			echo "No player with this id in the DBB";	
			$okToLoadPlayerQuest=false;	
		}
		
		//check if there are some inventory for our player in the inventory table already
		$SQL5 = "SELECT * FROM kr_quest_player WHERE player_id = '" . $playerid . "'";
		$result_id5 = @mysqli_query($database,$SQL5) or die("DATABASE ERROR!");
		$total5 = mysqli_num_rows($result_id5);
		if($total5) 
		{
			$emptyPlayerQuests=false;			
		}
		else 
		{
			$emptyPlayerQuests=true;
		}
	}
}


if($okToLoadPlayerQuest==true)   
{
	//creating new inventory in the table if the user is new		
	if($emptyPlayerQuests==true)
	{
			echo "vide";
	}
	else
	{

		//getting inventory data for the user
		$SQL3 = "SELECT * FROM kr_quest_player WHERE player_id = '" . $playerid . "'";
		$result_id3 = @mysqli_query($database,$SQL3) or die("DATABASE ERROR!");
		$total3 = mysqli_num_rows($result_id3);
		$myObjArray=array();
		
		if($total3) 
		{		
			while($row = mysqli_fetch_assoc($result_id3)){
				//echo "row id soil : ".$row["id_soil"]." -- ";
				
				//json conversion of the array					
				$myObj = new stdClass();
				
				$myObj->id_quest_player = $row["id_quest_player"];
				$myObj->player_id = $playerid;
				$myObj->id_quest = $row["id_quest"];
				$myObj->quest_completed = $row["quest_completed"];
				$myObj->begin_date = $row["begin_date"];
				$myObj->completion_date = $row["completion_date"];
				
				$myObjArray[] = $myObj;

			}
			$myJSON = json_encode($myObjArray);
			echo $myJSON;
		}
		else {
			echo "Data invalid - cant find any quests for player_id.";
		}
	}
}

// Close mySQL Connection
mysqli_close($database);
?>