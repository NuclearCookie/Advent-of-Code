package main

import "time"

type timeEvent interface {
	timestamp() time.Time
}
