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
			$okToLoadQuestTemplate=true;						
		}
		else 
		{
			echo "No player with this id in the DBB";	
			$okToLoadQuestTemplate=false;	
		}
		
	}
}


if($okToLoadQuestTemplate==true)   
{

	//getting quest_templates data 
	$SQL3 = "SELECT * FROM kr_quest_templates";
	$result_id3 = @mysqli_query($database,$SQL3) or die("DATABASE ERROR!");
	$total3 = mysqli_num_rows($result_id3);
	$myObjArray=array();
	
	if($total3) 
	{		
		while($row = mysqli_fetch_assoc($result_id3)){
			//echo "row id soil : ".$row["id_soil"]." -- ";
			
			//json conversion of the array					
			$myObj = new stdClass();
			
			$myObj->id_quest = $row["id_quest"];
			$myObj->questLore = $row["questLore"];
			$myObj->kindOfQuest = $row["kindOfQuest"];
			$myObj->objectiveItem = $row["objectiveItem"];
			$myObj->objectiveValue = $row["objectiveValue"];
			$myObj->rewardItem = $row["rewardItem"];
			$myObj->rewardValue = $row["rewardValue"];
			$myObj->multiplierXP = $row["multiplierXP"];
			$myObj->active = $row["active"];
			$myObj->tutorialQuestNumber = $row["tutorialQuestNumber"];
			$myObjArray[] = $myObj;

		}
		$myJSON = json_encode($myObjArray);
		echo $myJSON;
	}
	else {
		echo "Data invalid - cant find any quest template";
	}
}

// Close mySQL Connection
mysqli_close($database);
?>