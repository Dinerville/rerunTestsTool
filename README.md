The tool is console application as mstest, but you need to put some extra parametres to execute this tool
Here you should execute MSTest.Console.Extended.exe with following parametres

/resultsfile - path to the result file.
/retriesCount - how many times to rerun. For example if it is 1, failed tests will be re-execute 1 time
/deleteOldResultsFiles - better set to true
/newResultsfile - you can put any value here, it is not using in tool, just need to be not empty
/testcontainer - path to test dll file
/category - which category to run
/threshold - rerunning will work only if percent of failed tests less than percent in threshold. For example, if you don't want to rerun tests if more than 50% failed, you can specify /threshold:50
/testsettings - path to test settings file
NOTE you can use any mstest parametres in rerun tool. For example \nologo or \test:testName will work

Tool is based on this tool https://github.com/angelovstanton/AutomateThePlanet/tree/master/MSTest.Console
Difference is my tool saves multiple trx files for each run instead of 1 result file