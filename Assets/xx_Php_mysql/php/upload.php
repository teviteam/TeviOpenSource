<?php
include("config_inc.php");
// CONNECTIONS =========================================================
$database = mysqli_connect($host, $user, $password,$dbname,$dbport) or die('Could not connect: ' . mysqli_error());

//error_reporting(E_ALL);

echo " debut php ";

$dir = dirname(__FILE__);
echo "<p>Full path to this dir: " . $dir . "</p>";

if(isset($_POST["image_name"])) {
	$image_name=$_POST["image_name"];
}
	
$target_dir = 'images_upload/';
$target_file = $target_dir . basename($_FILES["fileToUpload"]["name"]);
$finalImageName = $image_name.".". pathinfo($_FILES['fileToUpload']['name'],PATHINFO_EXTENSION);

echo " target_file : ".$target_file. " ";
$uploadOk = 1;
$imageFileType = strtolower(pathinfo($target_file,PATHINFO_EXTENSION));
// Check if image file is a actual image or fake image
if(isset($_POST["submit"])) {
	echo " + test + ";
    $check = getimagesize($_FILES["fileToUpload"]["tmp_name"]);
    if($check !== false) {
        echo "File is an image - " . $check["mime"] . ".";
        $uploadOk = 1;
    } else {
        echo "File is not an image.";
        $uploadOk = 0;
    }
}
/*
// Check if file already exists
if (file_exists($target_file)) {
    echo "Sorry, file already exists.";
    $uploadOk = 0;
}*/

// Check file size
if ($_FILES["fileToUpload"]["size"] > 2000000) {
    echo "Sorry, your file is too large.";
    $uploadOk = 0;
}
// Allow certain file formats
if($imageFileType != "jpg" && $imageFileType != "png" && $imageFileType != "jpeg"
&& $imageFileType != "gif" ) {
    echo "Sorry, only JPG, JPEG, PNG & GIF files are allowed.";
    $uploadOk = 0;
}
// Check if $uploadOk is set to 0 by an error
if ($uploadOk == 0) {
    echo "Sorry, your file was not uploaded.";
// if everything is ok, try to upload file
} else {
	echo "so far so good";
	echo " file to upload ".$_FILES["fileToUpload"]["tmp_name"];
	echo " target ".$target_dir.$finalImageName;
    if (move_uploaded_file($_FILES["fileToUpload"]["tmp_name"], $target_dir.$finalImageName)) {
        echo "The file ". $target_dir.$finalImageName. " has been uploaded.";
    } else {
        echo "Sorry, there was an error uploading your file.";
    }
}

// Close mySQL Connection
mysqli_close($database);
?>