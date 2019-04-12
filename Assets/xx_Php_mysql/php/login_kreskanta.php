<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);
 
$email = anti_injection_login($_POST["myform_email"]); //I use that function to protect against SQL injection
$pass = anti_injection_login_senha($_POST["myform_pass"]);

if(!$email || !$pass) {
    echo "Login or password cant be empty.";
} else {
    if ($unityHash != $phpHash){
        echo "HASH code is diferent from your game, you infidel.";
    } else {
        $SQL = "SELECT * FROM kr_users WHERE playerEmail = '" . $email . "'";
        $result_id = @mysqli_query($database,$SQL) or die("DATABASE ERROR!");
        $total = mysqli_num_rows($result_id);
        if($total) {
            $datas = @mysqli_fetch_array($result_id);
            if($pass == $datas["playerPassword"]) 
			{
                echo "yipiiie";
				//update last connexion datetime
				$lastconnexion=date("Y-m-d H:i:s");
				$SQL = "UPDATE kr_users SET playerLastConnexion = '".$lastconnexion."' WHERE playerEmail = '" . $email . "'";
				$result_id = @mysqli_query($database,$SQL) or die("DATABASE ERROR UPDATE LAST CONNEXION");	
            }
			else {
                echo "email or password is wrong.";
            }
        } else {
            echo "Data invalid - cant find name.";
        }
    }
}
// Close mySQL Connection
mysqli_close($database);
?>