<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$playerid = anti_injection_login($_POST["myform_playerid"]); 
$idquest = anti_injection_login($_POST["myform_idquest"]); 
$playerquestid=0;

if ($unityHash != $phpHash)
{
	echo "HASH code is diferent from your game, you infidel.";
} 
else
{
	//getting quest id for this player quest
	$SQL3 = "SELECT * FROM kr_quest_player WHERE player_id = " . $playerid . " AND id_quest = " . $idquest . " ";
	//echo $SQL3;
	$result_id3 = @mysqli_query($database,$SQL3) or die("DATABASE ERROR!");
	$total3 = mysqli_num_rows($result_id3);
	
	if($total3) 
	{		
		while($row = mysqli_fetch_assoc($result_id3)){
			
			$playerquestid= $row["id_quest_player"];
		}
		echo $playerquestid;
	}
	else {
		echo "Data invalid - cant find any quests for player_id.";
	}
}
// Close mySQL Connection
mysqli_close($database);
?>