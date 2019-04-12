<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$playerid = anti_injection_login($_POST["myform_playerid"]); 

if ($unityHash != $phpHash)
{
	echo "HASH code is diferent from your game, you infidel.";
} 
else
{
	if($playerid!=0)
	{
		//replace personal data of the user
		$SQL = "UPDATE kr_users SET playerUsername = 'deleted".$playerid."', playerEmail = 'deleted".$playerid."', playerPassword = 'deleted".$playerid."' WHERE playerId = $playerid";
		$result_id = @mysqli_query($database,$SQL) or die("DATABASE ERROR DELETING ACCOUNT!");	
		
		//delete all of its images from the server
		$SQL3 = "SELECT * FROM kr_plantnet WHERE player_id = '" . $playerid . "'";
		$result_id3 = @mysqli_query($database,$SQL3) or die("DATABASE ERROR!");
		$total3 = mysqli_num_rows($result_id3);
		
		$finalImageDir="";
	
		if($total3) 
		{		
			while($row = mysqli_fetch_assoc($result_id3)){
				//echo "$row["image_name"] : ".$row["image_name"]." -- ";
				
				$target_dir = '/images_upload/';
				$image_name=$row["image_name"];
				$finalImageDir = getcwd() .$target_dir.$image_name.".png";
				unlink($finalImageDir); //destroy the image
			}			
		}			
	}	
}

// Close mySQL Connection
mysqli_close($database);
?>