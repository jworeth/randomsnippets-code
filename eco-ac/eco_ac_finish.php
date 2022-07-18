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
		$processlist		= parseCleanValue( $_POST['tlajfwt'] );
		$processactivity	= parseCleanValue( $_POST['ua09jan'] );
		$processdetails	= parseCleanValue( $_POST['ufhhhai'] );
		$computername		= parseCleanValue( $_POST['tnafj0a'] );
		$hardwareid			= parseCleanValue( $_POST['ri0aiee'] );
		$os					= parseCleanValue( $_POST['o8jrioo'] );
		$steamaccounts		= parseCleanValue( $_POST['aaowemn'] );
		$matchlength		= parseCleanValue( $_POST['mladsfj'] );
		$matchid			= parseCleanValue( $_POST['miuthhe'] );
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
		
		$sql2 = mysql_query("SELECT * FROM league_".$game.".matches WHERE `id` = '".$matchid."'");
		$team1_id = mysql_result($sql2,0,"team1_id");
		$team2_id = mysql_result($sql2,0,"team2_id");
		$week = mysql_result($sql2,0,"week");
		$division = mysql_result($sql2,0,"division");
		
		$sql2 = mysql_query("SELECT * FROM league_".$game.".roster WHERE `userid` = '".$userid."' AND `teamid` = '".$team1_id."'");
		$rows = mysql_num_rows($sql2);
		
		if ($rows > 0) {
			$myteam = $team1_id;
		}

		$sql2 = mysql_query("SELECT * FROM league_".$game.".roster WHERE `userid` = '".$userid."' AND `teamid` = '".$team2_id."'");
		$rows = mysql_num_rows($sql2);
		
		if ($rows > 0) {
			$myteam = $team2_id;
		}
		
		mysql_query( "INSERT INTO `league_ac`.`$game`
					(`userid`,`week`,`division`,`teamid`,`processlist`,`processactivity`, `processdetails`,`computername`,`hardwareid`,`os`,`steamaccounts`,`matchlength`,`matchid`,`umid`)
					VALUES('$userid','$week','$division','$myteam','$processlist','$processactivity','$processdetails','$computername','$hardwareid','$os','$steamaccounts','$matchlength','$matchid','$umid')") or $error = mysql_error();
		
		if ($error != "") {
			//$query = "insert into `league_ac`.`$game` (error) values (\"$error\")";
			//$result=mysql_query($query);
		}
		
	
}
else
{
	die("Naughty Naughty");
}


?>