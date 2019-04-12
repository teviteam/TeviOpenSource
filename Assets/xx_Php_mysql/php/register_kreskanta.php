<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());
// =============================================================================
$unityHash = anti_injection_login($_POST["myform_hash"]);

$jsondata = $_POST["myform_jsonregister"]; 

$myObj = new stdClass();
$myObj =json_decode($jsondata, TRUE);

$okToRegister =false;

//echo $myObj;

foreach ($myObj as $key => $val) {
	//echo $key;
	if ($unityHash != $phpHash)
	{
		echo "HASH code is diferent from your game, you infidel.";
	} 
	else
	{
		if($key=="playerEmail")
		{
			//echo $val;
			//echo "so far so good";
			$SQL2 = "SELECT * FROM kr_users WHERE playerEmail = '" . $val . "'";
			$result_id = @mysqli_query($database, $SQL2) or die("DATABASE ERROR!");
			$total = mysqli_num_rows($result_id);
			if($total) 
			{
				echo "Someone already registered with this email. For forgotten password, please write to us.";            
			}
			else 
			{
				$okToRegister=true;			
			}
		}
		if($key=="playerPassword")
		{
			$val2=md5($val);
			$myObj[$key]=$val2;
		}
	}
}

if($okToRegister==true)   
{
	foreach ($myObj as $key => $val) {

		$sql2[] = (is_numeric($val)) ? "`$key` = $val" : "`$key` = '" . mysqli_real_escape_string($database, $val) . "'";
	}
	$sqlclause = implode(",",$sql2);
	//echo $sqlclause;
	$rs = mysqli_query($database, "INSERT INTO `kr_users` SET $sqlclause") or die("DATABASE ERROR REGISTERING!");
	echo "yipiiie register";
}


// Close mySQL Connection
mysqli_close($database);
?>