<?php
// CONNECTIONS =========================================================
$host = ""; //put your host here
$user = ""; //in general is root
$password = ""; //use your password here
$dbname = ""; //your database
$dbport = ini_get("mysqli.default_port");
$phpHash = ""; // same code in here as in your Unity game

// =============================================================================
// PROTECT AGAINST SQL INJECTION and CONVERT PASSWORD INTO MD5 formats
function anti_injection_login_senha($sql, $formUse = true)
{
	$sql = preg_replace("/(from|select|insert|delete|where|drop table|show tables|,|'|#|\*|--|\\\\)/i","",$sql);
	$sql = trim($sql);
	$sql = strip_tags($sql);
	if(!$formUse || !get_magic_quotes_gpc())
	{
		$sql = addslashes($sql);
		$sql = md5(trim($sql));
	}
	return $sql;
}
// THIS ONE IS JUST FOR THE emailNAME PROTECTION AGAINST SQL INJECTION
function anti_injection_login($sql, $formUse = true)
{
	$sql = preg_replace("/(from|select|insert|delete|where|drop table|show tables|,|'|#|\*|--|\\\\)/i","",$sql);
	$sql = trim($sql);
	$sql = strip_tags($sql);
	if(!$formUse || !get_magic_quotes_gpc())
	{
		$sql = addslashes($sql);
	}
	return $sql;
}
// ====
?>