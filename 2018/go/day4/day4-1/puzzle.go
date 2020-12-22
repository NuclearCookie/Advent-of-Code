package main

import (
	"fmt"
	"time"

	"github.com/nuclearcookie/aoc2018/day4/common"
	"github.com/nuclearcookie/aoc2018/day4/input"
)

func main() {
	startTime := time.Now()
	data := input.GetSplit()
	guards := common.ProcessGuards(data)
	sleepyHead := common.FindMostSleepyGuard(&guards)
	mostSleepyMinute, _ := common.FindMostSleepyMinute(&sleepyHead)

	duration := time.Since(startTime)
	fmt.Printf("Duration: %s\n", duration)
	fmt.Printf("Solution: id: %d * minute: %d = %d", sleepyHead.ID, mostSleepyMinute, sleepyHead.ID*mostSleepyMinute)
}
