<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$jsondata = $_POST["myform_playerquest"]; 

$myObj2 = new stdClass();
$myObj2 =json_decode($jsondata, TRUE);

$player_id=0;

$idQuest=0;
$sqlclause="";
$sql2=array(); //important to empty the request and avoid having two items at a time.
$sql3=array();
$updateQuest=false;			

//parsing content of the quest
foreach ($myObj2 as $key => $val) {
	//verifying if it is a new user or  not
	if($key=="id_quest_player")
	{
		if ($val!=0)
		{
			$idQuest=$val;
		}
		
		//echo $val;
		$SQL = "SELECT * FROM kr_quest_player WHERE id_quest_player = '" . $val . "'";
		$result_id = @mysqli_query($database,$SQL) or die("DATABASE ERROR FINDING QUEST!");
		$total = mysqli_num_rows($result_id);
		if($total) 
		{
			//echo "quest found in the database => updating";	
			$updateQuest=true;						
		}

	}			
	$sql2[] = (is_numeric($val)) ? "`$key` = $val" : "`$key` = '" . mysqli_real_escape_string($database,$val) . "'";
}

//actually updating 
if($updateQuest==true)
{
	$sqlclauseupdate = implode(",",$sql2);
	$rs = mysqli_query($database,"UPDATE `kr_quest_player` SET $sqlclauseupdate WHERE id_quest_player = ".$idQuest) or die("DATABASE ERROR UPDATING QUEST!");
}		
	
echo "update player quest done";


// Close mySQL Connection
mysqli_close($database);
?>