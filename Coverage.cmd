dotnet test --logger "trx;logfilename=TestResults123.trx" ^
			--results-directory ./Coverage ^
			/p:CollectCoverage=true ^
			/p:CoverletOutput=../Coverage/ ^
			/p:CoverletOutputFormat=cobertura


reportgenerator -reports:.\Coverage\coverage.cobertura.xml  -targetdir:.\Coverage
