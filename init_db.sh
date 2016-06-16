#!/bin/bash

until ./load_data.sh; do
	echo "Try again"
	sleep 1
done
