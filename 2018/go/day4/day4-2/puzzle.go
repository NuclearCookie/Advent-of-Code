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
	maxTimesAsleep := 0
	mostSleepyMinute := -1
	var sleepyHead common.Guard
	for _, guard := range guards {
		mostSleepyMinuteLocal, maxTimesAsleepLocal := common.FindMostSleepyMinute(&guard)
		if maxTimesAsleepLocal > maxTimesAsleep {
			maxTimesAsleep = maxTimesAsleepLocal
			mostSleepyMinute = mostSleepyMinuteLocal
			sleepyHead = guard
		}

	}

	duration := time.Since(startTime)
	fmt.Printf("Duration: %s\n", duration)
	fmt.Printf("Solution: id: %d * minute: %d = %d", sleepyHead.ID, mostSleepyMinute, sleepyHead.ID*mostSleepyMinute)
}
