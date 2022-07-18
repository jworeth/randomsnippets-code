<?php
error_reporting(0);
require_once ('../includes/common/cleanInput.php');
require_once ('../includes/common/getDbase.php');
require_once ('../includes/common/all_leagues.php');

// Define $myusername and $mypassword
$userid = $_GET['userid'];
$password = $_GET['password'];

if($userid == NULL || $password == NULL){
header('Location: 404.html');
die();
}

// To protect MySQL injection
$userid = stripslashes($userid);
$password = stripslashes($password);
$userid = mysql_real_escape_string($userid);
$password = mysql_real_escape_string($password);


$query = "select salt from league_shared.users where userid = '$userid'";
$result=mysql_query($query);
$salt_row = mysql_fetch_array($result);

$password = md5(md5($password) . $salt_row['salt']);

$query = "SELECT userid FROM league_shared.users WHERE userid ='$userid' and password='$password'";
$result=mysql_query($query);
$count=mysql_num_rows($result);

/*
if($count!=1){//success requires only one row returned
header('Location: 404.html');
die();
} else{

*/
// This will be where the chet processes are
echo "Steam;".
"sucks;".
"ass;";
//}


?>