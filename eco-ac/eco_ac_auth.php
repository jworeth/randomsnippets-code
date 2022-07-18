<?php

require_once("../includes/common/getDbase.php");

function verifyInput( $inputString, $method = 'post' )
{
	if ( $method = 'post' )
	{
		$data = $_POST;
	}
	elseif ( $method = 'get' )
	{
		$data = $_GET;
	}
	
	$failed = false;
	
	$toCheck = explode( ",", $inputString );
	
	foreach( $toCheck as $k => $v )
	{
		if ( empty( $toCheck[ $k ] ) || $v == '' )
		{
			$failed = true;
		}
	}
	
	return $failed;
}

function parseCleanValue( $v )
{
	if ( get_magic_quotes_runtime())
	{
		$v = stripslashes( $v );
	}
	
	//$v = htmlentities( $v); wasn't blocking ' char
	
	$v = htmlspecialchars($v, ENT_QUOTES);

	return $v;
}

//$error = "No Error (Matchid: ".$_POST['miuthhe']." and userid: ".$_POST['gv14789'].") (GameID: ".$_POST['siueeqa'].")";
//$query = "insert into `league_ac`.`css` (error) values (\"GameID: ".$_POST['siueeqa']."\")";
//$result=mysql_query($query);


if ( $_SERVER['HTTP_USER_AGENT'] == '0x0100' )
{
	
		$userid 			= parseCleanValue( $_POST['gv14789'] );
		$matchid			= parseCleanValue( $_POST['miuthhe'] );
		$networkactivity 	= parseCleanValue( $_POST['h54sbs'] );
		$game				= parseCleanValue( $_POST['siueeqa'] );
		
		$umid = sha1(substr(sha1($matchid ), 5, 10) . substr(sha1($userid ), 5, 10));

		switch($game){
			case 1:
				$game = "css";
				break;
			case 2:
				$game = "cs16";
				break;
			case 3:
				$game = "dota";
				break;
			default:
				break;
		}

				
		
		mysql_query( "insert into `league_ac`.`$game` (userid, matchid, networkactivity, umid, login_time) values ('$userid', '$matchid', '$networkactivity', '$umid', CURRENT_TIMESTAMP)") or $error = mysql_error();
		

		if ($error != "") {
			$query = "insert into `league_ac`.`$game` (error) values (\"$error\")";
			$result=mysql_query($query);
		}
		
	
}
else
{
	die("Naughty Naughty");
}


?>