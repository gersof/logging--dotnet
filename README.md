# logging--dotnet
The main idea is  evaluate different kind of logger for Dotnet Core
This one file is the result  of logging process https://raw.githubusercontent.com/gersof/logging--dotnet/logging-serilog/BACKEND/Invoice.WebAPI/logs/log20200923.txt

How to run?
- You must restore the SQL Script
- Change Connection string in AppSetting.Json
- Start the web project 
- For test to https://localhost:44320/swagger/index.html

If you want to use Seq is necesary that you install or use docker https://hub.docker.com/r/datalust/seq
- In docker execute the next command  docker run --name seq -d --restart unless-stopped  -e ACCEPT_EULA=Y  -v /path/to/seq/data:/data  -p 8182:80 -p 5341:5341  datalust/seq:latest

After that  you can access to the viewer logs  in the next url http://localhost:8182/#/events
