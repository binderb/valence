<?php

  // Strings must be escaped to prevent SQL injection attack.
  $to = $_POST["myEmail"];
  $subject = $_POST["mySubject"];
  $message = $_POST["myMessage"];

  // Add headers
  $headers = 'From: valence_client@benbinder.com' . "\r\n" .
             'Reply-To: valence@benbinder.com' . "\r\n" .
             'X-Mailer: PHP/' . phpversion();

  // Send message
  mail($to, $subject, $message, $headers);

?>
