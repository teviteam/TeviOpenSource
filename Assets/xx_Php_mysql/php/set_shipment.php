<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$playerid = anti_injection_login($_POST["myform_playerid"]); 
$playerLastShipment = $_POST["myform_playerLastShipment"]; 

if ($unityHash != $phpHash)
{
	echo "HASH code is diferent from your game, you infidel.";
} 
else
{
	if($playerid!=0)
	{
		//update last shipment date 
		$SQL = "UPDATE kr_users SET playerLastShipment = '".$playerLastShipment."' WHERE playerId = '".$playerid."'";
		$result_id = @mysqli_query($database,$SQL) or die("DATABASE ERROR UPDATING SHIPMENT!");	

				
	}	
}

// Close mySQL Connection
mysqli_close($database);
?>