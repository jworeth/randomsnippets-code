<?php

$fp = fopen('tests.gif', 'wb' );
fwrite( $fp, $GLOBALS[ 'HTTP_RAW_POST_DATA' ] );
fclose( $fp );

?>