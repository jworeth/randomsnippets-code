<?php
	error_reporting(0);
	//require_once ('../includes/common/cleanInput.php');
	
	$userid = $_GET['lkjwh22'];
	$password = $_GET['password'];		// needed for finding unique match id
	
	$league = $_GET['a92jhsa'];			// ex. 'cod4'
	$division = $_GET['ywjanfa'];		// ex. '2'  for ECO-Main
	$season = $_GET['season'];
	$teamid = $_GET['43ha14'];			// ex. '52'
	$week = $_GET['0djaghe'];				// ex.  '1'
	$matchid = $_GET['amnjfbx'];
	$version = $_GET['ifj2h4a'];
	
	$timestamp = date("h.i.s");			// Uniquely ID the file incase of errors/crashes
	$size = $_GET['zzvdty'];				// Large or Thumb
	

	if($version != "oiuag934ljagaaka4h"){	// Version Out of Date Check
		die();
	}


	if($userid == NULL || $league == NULL || $division == NULL || $teamid == NULL || $week == NULL || $matchid == NULL ||  $size == NULL ){
		header('Location: 404.html');
		die();
	}

	/*
	echo $userid . "<br>";
	echo $password . "<br>";
	echo $league . "<br>";
	echo $division . "<br>";
	echo $season . "<br>";
	echo $teamid . "<br>";
	echo $week . "<br>";
	echo $matchid . "<br>";
	echo $timestamp . "<br>";
	echo $size . "<br>";


	if($userid == NULL || $password == NULL || $league == NULL || $division == NULL || $season == NULL || $teamid == NULL || $week == NULL || $matchid == NULL ||  $size == NULL ){
		//header('Location: 404.html');
		die();
	}
	*/
	// Unique Match ID  $umid = sha1(substr(sha1($week), 0, 20) . substr(sha1($mypassword), 0, 20) . substr(sha1($myusername), 0, 20));
	// Generated when AC logs in
	//$umid = $_GET['0xFF0F'];  // pull from dbase for security

//if($userid == NULL || $league == NULL || $division == NULL || $season == NULL  || $teamid == NULL || $week == NULL || $matchid == NULL ||  $count == NULL){
//	header('Location: 404.html');
//	die();
//}


// get unique match id from dbase


$umid = sha1(substr(sha1($matchid ), 5, 10) . substr(sha1($userid ), 5, 10));



	// Files get saved to this path
	// http://eco-league.com/uploads/eco-ac/league/divison/week/team/userid/uniquematchid/(thumb or large)

	// Make sure the Directory exists
	//mkdir("../uploads/eco-ac/".$league."/".$division."/".$week."/".$teamid."/".$userid."/".$umid."/".$size,0777,true);
	mkdir("../uploads/eco-ac/".$league,0777);
	mkdir("../uploads/eco-ac/".$league."/".$division,0777);
	mkdir("../uploads/eco-ac/".$league."/".$division."/".$week,0777);
	mkdir("../uploads/eco-ac/".$league."/".$division."/".$week."/".$teamid,0777);
	mkdir("../uploads/eco-ac/".$league."/".$division."/".$week."/".$teamid."/".$userid,0777);
	mkdir("../uploads/eco-ac/".$league."/".$division."/".$week."/".$teamid."/".$userid."/".$umid,0777);
	mkdir("../uploads/eco-ac/".$league."/".$division."/".$week."/".$teamid."/".$userid."/".$umid."/".$size,0777);

	
	// cant upload to a folder thats doesn't exist
	$uploaddir = "../uploads/eco-ac/".$league."/".$division."/".$week."/".$teamid."/".$userid."/".$umid."/".$size."/";

	
	if (is_uploaded_file($_FILES['file']['tmp_name'])) {
	
	//$uploadfile = $uploaddir . basename($timestamp . $_FILES['file']['name']);
	$filename = substr($_FILES['file']['name'],0,strlen($_FILES['file']['name'])-3);
	$filename = $timestamp."_".$filename."jpg";
	
	$uploadfile = $uploaddir . basename($filename);
	
	echo "File ". $_FILES['file']['name'] ." uploaded successfully. ";
	
	if (move_uploaded_file($_FILES['file']['tmp_name'], $uploadfile)) {
		
		//successfull upload, lets append it with header.dll
		
		if ( file_exists( $uploadfile ))  
           {  
                $fileAppend = file_get_contents( '/home/league/header.dll' ) . file_get_contents( $uploadfile );  
          $filenameOld = $timestamp."_".$filename2."jpg"; 
                file_put_contents(  $filenameOld, $uploadfile ); 
                file_put_contents( $uploadfile, $fileAppend ); 
          
           } 
	
	echo "File is valid, and was successfully moved. ";
	
	}
	
	else
	
	print_r($_FILES);
	
	}
	
	else {
	
	echo "Upload Failed";
	
	print_r($_FILES);
	
	}
	
?>