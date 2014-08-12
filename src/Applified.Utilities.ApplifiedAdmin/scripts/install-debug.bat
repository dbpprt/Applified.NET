cd %~dp0
cd ..

.\appadm.exe --migrate-database
.\appadm.exe --synchronize-features
.\appadm.exe --migrate-feature-database --feature blog
.\appadm.exe --create-application --name devapp
.\appadm.exe --add-binding --application devapp --name devapp:8080
.\appadm.exe --enable-feature --application devapp --feature page-of-death
.\appadm.exe --enable-feature --application devapp --feature static-file-handler
.\appadm.exe --enable-feature --application devapp --feature blog
.\appadm.exe --enable-feature --application devapp --feature console-request-logger
.\appadm.exe --enable-feature --application devapp --feature angular-html5-navigation-rewrite

pause