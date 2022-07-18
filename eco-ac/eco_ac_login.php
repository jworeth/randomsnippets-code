<?php
	error_reporting(0);
	require_once ('../includes/common/cleanInput.php');
	require_once ('../includes/common/getDbase.php');
	require_once ('../includes/common/all_leagues.php');
	
if ( $_SERVER['HTTP_USER_AGENT'] == '0x0100' )
{

	// Define $myusername and $mypassword
	$userid = $_GET['9fhabej'];
	$password = $_GET['irjwb42'];
	$version = $_GET['ioura11'];
	
		
	if($userid == NULL || $password == NULL){
		header('Location: 404.html');
		die();
	}
	

	if($version != "9e7h67hrgs8erhd"){	// Version Out of Date Check
		echo "89fah22h";
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
	
	//$password = md5(md5($password) . $salt_row['salt']);
	

	//$password = sha1(substr(sha1($password ), 0, 20) . substr(sha1($userid ), 0, 20));
	
	/* More effecient way but not implemented yet, Store a users teamid in the users table
	echo $userid ." " . $salt_row['salt'] . " " . $password . "<br>";
	$query = "SELECT teamid_css, teamid_cs16, teamid_cscz, teamid_dota, teamid_cod4 FROM league_shared.users WHERE userid ='$userid' and password='$password'";
	$result=mysql_query($query);
	*/

	$query = "SELECT userid FROM league_shared.users WHERE userid ='$userid' and password='$password'";
	$result=mysql_query($query);
	$count=mysql_num_rows($result);
	
	$sql="SELECT * FROM league_forum.vbul_user WHERE username ='$userid'";
	$result=mysql_query($sql);
	$b_row = mysql_fetch_array($result);
	
	$mypassword = md5(md5($password) . $b_row['salt']);

	if($mypassword != $b_row['password']) {  //successfull login
		echo "0gfgjeksa"; // Bad Login
		die();
	} else {
	
	/* Not implemented yet
	$a_row = mysql_fetch_array($result);
	$teamid_css = $a_row[teamid_css];
	$teamid_cs16 = $a_row[teamid_cs16];
	$teamid_cscz = $a_row[teamid_cscz];
	$teamid_dota = $a_row[teamid_dota];
	$teamid_cod4 = $a_row[teamid_cod4];
	*/
	
	
	// Get All leagues and divisions
	$all_leagues = array();
	
	$query = "select * from league_shared.games where type = '1'";
	$result=mysql_query($query);
	while($leagues_row = mysql_fetch_array($result))
	{
		$all_leagues[] = array($leagues_row['textid'], $leagues_row['name']);
	}
	


	$matchcount=0;
	$x=0;
	foreach($all_leagues as $league){
	
		//echo $league[0] . "<br>";
	
		$query = "select teamid from league_{$league[0]}.roster where userid = '$userid' and status in ('0','1')";
		$result2=mysql_query($query);
		while($division_row = mysql_fetch_array($result2))
		{

		//echo $division_row['teamid'];
		$teamid = $division_row['teamid'];
		
		switch($league[0]){
			case css:
				$leaguenumber = 1;
				break;
			case cs16:
				$leaguenumber = 2;
				break;
			case dota:
				$leaguenumber = 3;
				break;
			default:
				break;
		}
		
		
		
		$query = "SELECT id, division, season, week, map, team1_name, team2_name FROM league_".$league[0].".matches WHERE team1_id = '$teamid' and reportdate is NULL or team2_id = '$teamid' and reportdate is NULL";
		$result=mysql_query($query);
		
		//echo $query;
	
			$count=mysql_num_rows($result);
					//echo "<br>" . $league[0] . " " . $league[1] . " " . $count . " " . $teamid;
			if($count>=1){		//success requires only one row returned
				
				while($a_row = mysql_fetch_array($result))
				{
				
					$t1name = str_replace(";", "", $a_row[team1_name]);
					$t1name  = str_replace(",", "", $t1name );
					$t2name  = str_replace(";", "", $a_row[team2_name]);
					$t2name = str_replace(",", "", $t2name);
				
					$matchcount++;	// determine if any matches needs played
					if($x!=0){ echo ";"; }
					echo 
					$a_row[id].",".
					$leaguenumber.",".
					$a_row[division].",".
					$a_row[season].",".
					$a_row[week].",".
					$a_row[map].",".
					$t1name.",".
					$t2name.",".
					$league[1].",".
					$teamid.",".
					"eco-league.com";
					$x++;
				}
			}

	
		} // end while
	
	}	// end for
	
	if($matchcount == 0){
		echo "89fah22h";
		die();
	}
	
	
	}
	
}
?>