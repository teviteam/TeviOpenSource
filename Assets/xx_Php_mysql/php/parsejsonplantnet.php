<?php

include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());


//echo "begin parsing json";
if(isset($_POST["myform_urltoparse"])) {

	$url = $_POST["myform_urltoparse"]; 
	$playerid=$_POST["myform_playerid"];
	$image_name=$_POST["myform_tempImageName"];
	$target_dir = '/images_upload/';
	$finalImageDir = getcwd() .$target_dir.$image_name.".png";
	$jsondata = @file_get_contents($url);	//ignore warning if there is
}

if($jsondata===FALSE) //check if we have a 404 error on this request
{
	echo "not a plant";
	//echo $finalImageDir;
	unlink($finalImageDir); //destroy the image
}
else
{
	$arr = json_decode($jsondata, true);

	// Print a single value
	/*
	echo " - ";  
	echo $arr["results"][0]["score"] ; 
	echo $arr["results"][0]["species"]["scientificNameWithoutAuthor"] ; 
	echo $arr["results"][0]["species"]["commonNames"][0] ; 
	echo " - "; */

	if ($arr["results"][0]["score"]>5)
	{			
		//saving results to table
		$plant0=(string)$arr["results"][0]["species"]["scientificNameWithoutAuthor"];
		$plant0=mysqli_real_escape_string($database,$plant0);
		$common0=(string)$arr["results"][0]["species"]["commonNames"][0];
		$common0=mysqli_real_escape_string($database,$common0);
		$score0=(string)$arr["results"][0]["score"];		
		$plant1=(string)$arr["results"][1]["species"]["scientificNameWithoutAuthor"];
		$plant1=mysqli_real_escape_string($database,$plant1);
		$common1=(string)$arr["results"][1]["species"]["commonNames"][0];
		$common1=mysqli_real_escape_string($database,$common1);
		$score1=(string)$arr["results"][1]["score"];		
		$plant2=(string)$arr["results"][2]["species"]["scientificNameWithoutAuthor"];
		$plant2=mysqli_real_escape_string($database,$plant2);
		$common2=(string)$arr["results"][2]["species"]["commonNames"][0];
		$common2=mysqli_real_escape_string($database,$common2);
		$score2=(string)$arr["results"][2]["score"];	
		$currentTime = date("Y-m-d H:i:s");
		
		//echo "INSERT INTO `kr_plantnet` (id_plant,player_id,image_name,plant0,common0,score0,plant1,common1,score1,plant2,common2,score2,date) VALUES ('','".$playerid."','".$image_name."','".$plant0."','".$common0."','".$score0."','".$plant1."','".$common1."','".$score1."','".$plant2."','".$common2."','".$score2."','".$currentTime."') ";
		
		$rs = mysqli_query($database,"INSERT INTO `kr_plantnet` (id_plant,player_id,image_name,plant0,common0,score0,plant1,common1,score1,plant2,common2,score2,date) VALUES ('','".$playerid."','".$image_name."','".$plant0."','".$common0."','".$score0."','".$plant1."','".$common1."','".$score1."','".$plant2."','".$common2."','".$score2."','".$currentTime."') ") or die("DATABASE ERROR CREATING PLANTNET!");
		
		//creating the json data to transfer to Unity
		$myObjArray=array();
		$myObj = new stdClass();
		
		$myObj->id_plant = 0;
		$myObj->player_id = $playerid;
		$myObj->image_name = $image_name;	
		$myObj->plant0 = $arr["results"][0]["species"]["scientificNameWithoutAuthor"] ; 
		$myObj->common0 = $arr["results"][0]["species"]["commonNames"][0] ; 	
		$myObj->score0 = $arr["results"][0]["score"];
		$myObj->plant1 = $arr["results"][1]["species"]["scientificNameWithoutAuthor"] ; 
		$myObj->common1 = $arr["results"][1]["species"]["commonNames"][0] ; 	
		$myObj->score1 = $arr["results"][1]["score"];
		$myObj->plant2 = $arr["results"][2]["species"]["scientificNameWithoutAuthor"] ; 
		$myObj->common2 = $arr["results"][2]["species"]["commonNames"][0] ; 	
		$myObj->score2 = $arr["results"][2]["score"];
		$myObj->date = $currentTime;

		$myObjArray[] = $myObj;

		$myJSON = json_encode($myObjArray);
		echo $myJSON;
	}
	else{
		echo "not sure";
		//echo $finalImageDir;
		unlink($finalImageDir); //destroy the image
	}

}
mysqli_close($database);


?>