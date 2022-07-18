<?php
require_once ('../includes/common/cleanInput.php');

$directory = $_GET['imagefolder'];

dirList($directory);

echo $filearray[0];




function dirList ($directory) 
{

    // create an array to hold directory list
    $results = array();

    // create a handler for the directory
    $handler = opendir("/home/league/public_html" . $directory);

    // keep going until all files in directory have been read
    while ($file = readdir($handler)) {

        // if $file isn't this directory or its parent, 
        // add it to the results array
        if ($file != '.' && $file != '..'){
            $results[] = $file;
            echo "<img src='".$directory.$file."' /><br/>";
            }
    }

    // tidy up: close the handler
    closedir($handler);
    // done!
    


}
echo "Hi";

?>