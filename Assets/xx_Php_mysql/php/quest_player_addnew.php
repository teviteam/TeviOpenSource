<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$jsondata = $_POST["myform_playerquest"]; 

$myObj = new stdClass();
$myObj =json_decode($jsondata, TRUE);



foreach ($myObj as $key => $val) {

	$sql2[] = (is_numeric($val)) ? "`$key` = $val" : "`$key` = '" . mysqli_real_escape_string($database, $val) . "'";
}
$sqlclause = implode(",",$sql2);
echo $sqlclause;
$rs = mysqli_query($database, "INSERT INTO `kr_quest_player` SET $sqlclause ") or die("DATABASE ERROR ADDING PLAYER QUESTS!");
echo "yipiiie adding quest";

// Close mySQL Connection
mysqli_close($database);
?>