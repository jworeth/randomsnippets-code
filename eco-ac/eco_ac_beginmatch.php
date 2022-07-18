<?php
	error_reporting(0);
	require_once ('../includes/common/cleanInput.php');
	require_once ('../includes/common/getDbase.php');

	// Define $myusername and $mypassword
	$userid = $_GET['userid'];
	$password = $_GET['password'];
	$league = $_GET['league'];
	$division = $_GET['division'];
	$season = $_GET['season'];
	$week = $_GET['week'];
	$matchid = $_GET['matchid'];
	$teamid = $_GET['teamid'];


if($userid == NULL || $league == NULL || $division == NULL || $season == NULL  || $teamid == NULL || $week == NULL || $matchid == NULL){
	header('Location: 404.html');
	die();
}

	$time_stamp = date("Y-m-d H:i:s");
	$user_ip = $_SERVER['REMOTE_ADDR'];
	$user_host = gethostbyaddr($user_ip);
	
	
	// To protect MySQL injection
	$userid = stripslashes($userid );
	$password = stripslashes($password );
	$userid = mysql_real_escape_string($userid );
	$password = mysql_real_escape_string($password );
	
	$query = "select salt from league_shared.users where userid = '$userid'";
	$result=mysql_query($query);
	$salt_row = mysql_fetch_array($result);
	
	$password = md5(md5($password) . $salt_row['salt']);

	//$password = sha1(substr(sha1($password ), 0, 20) . substr(sha1($userid ), 0, 20));
	

	$query = "SELECT userid, password FROM league_shared.users WHERE userid ='$userid' and password='$password'";
	$result=mysql_query($query);
	

	$count=mysql_num_rows($result);
	
	if($count!=1){		//success requires only one row returned
		header('Location: 404.html');
		die();
	} else{
	

	switch($league){
		case 1:
			$league_name = "css";
			break;
		case 2:
			$league_name= "cs16";
			break;
		case 3:
			$league_name= "dota";
			break;
		case 4:
			$league_name= "cod4";
			break;
	}
	
	
	$umid = sha1(substr(sha1($matchid), 5, 10) . substr(sha1($password), 5, 10) . substr(sha1($userid ), 5, 10));
	$ss_dir = "/uploads/eco-acs/";
	$query = "insert into league_ac.".$league_name." (league,division,season,week,login_time,userid,umid,ss_dir,ip,hostname) values ('$league','$division','$season','$week','$time_stamp','$userid','$umid','$ss_dir','$user_ip','$user_host')";
	$result=mysql_query($query);
	
	
	}


?>