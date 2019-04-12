<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);
 
$email = anti_injection_login($_POST["myform_email"]); //I use that function to protect against SQL injection

if(!$email ) 
{	
	echo "Login cant be empty.";
} 
else 
{
	if ($unityHash != $phpHash)
	{
		echo "HASH code is diferent from your game, you infidel.";
	} 
	else 
	{		
		$SQL2 = "SELECT * FROM kr_users WHERE playerEmail = '" . $email . "'";
		$result_id2 = @mysqli_query($database,$SQL2) or die("DATABASE ERROR!");
		$total2 = mysqli_num_rows($result_id2);
		if($total2) 
		{
			
			$datas2 = @mysqli_fetch_array($result_id2);
			//json conversion of the array
						
			$myObj = new stdClass();
			
			$myObj->playerId = $datas2["playerId"];
			$myObj->playerUsername = $datas2["playerUsername"];
			$myObj->playerEmail = $email;
			$myObj->playerLvl = $datas2["playerLvl"];
			$myObj->playerCorp = $datas2["playerCorp"];
			$myObj->playerNumbSoilx = $datas2["playerNumbSoilx"];
			$myObj->playerNumbSoily = $datas2["playerNumbSoily"];
			$myObj->playerXP = $datas2["playerXP"];
			$myObj->playerAccountCreation = $datas2["playerAccountCreation"];
			$myObj->playerLastConnexion = $datas2["playerLastConnexion"];
			$myObj->playerLastShipment = $datas2["playerLastShipment"];

			$myJSON = json_encode($myObj);
			echo $myJSON;
			//echo "ok";
		}
		else {
            echo "Data invalid - cant find name.";
        }
	} 
}

// Close mySQL Connection
mysqli_close($database);
?>