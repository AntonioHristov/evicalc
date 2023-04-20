# evicalc

It's a calculator project with a server asp.net web core api, a client console and models in common.
When you execute the program, it welcomes you and waiting for a enter key press, 
then it shows you the main menu with the operations available which are:
add, sub, mult, div, sqr, see the journal and exit (1 character is available, the menu shows you).
The client send the request to the server, the server resolves it and return a response to the client.
The journal's data are store in a file which name is journal.txt, it's ubicated in the main folder, the same as this file
(if not exist the program will create it)
Also there's 2 log files (one for server and the other one from client) which store information about the program,
The log server file is ubicated in evicalc.server/logs/(date).log (if not exist the program will create it),
and the log client file is ubicated in evicalc.client/logs/(date).log (if not exist the program will create it).

