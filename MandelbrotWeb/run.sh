#!/usr/bin/env bash

dotnet watch run --urls "http://%computername%:5000;https://%computername%:5001;http://localhost:5000;https://localhost:5001"