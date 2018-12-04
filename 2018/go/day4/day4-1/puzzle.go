package main

import (
	"fmt"
	"regexp"
	"sort"
	"strconv"
	"time"

	"github.com/nuclearcookie/aoc2018/day4/input"
	"github.com/nuclearcookie/aoc2018/utils/sliceutils"
)

func main() {
	startTime := time.Now()
	data := input.GetSplit()
	guards := make(map[int]guard)
	c := make(chan guard)
	sort.Strings(data)
	go parse(data, c)
	for event := range c {
		_, exists := guards[event.id]
		if !exists {
			guards[event.id] = event
		} else {
			currentGuard := guards[event.id]
			for i, v := range event.asleepTimes {
				currentGuard.asleepTimes[i] += v
			}
		}
	}
	sleepyHead := findMostSleepyGuard(&guards)
	mostSleepyMinute := findMostSleepyMinute(&sleepyHead)

	duration := time.Since(startTime)
	fmt.Printf("Duration: %s\n", duration)
	fmt.Printf("Solution: id: %d * minute: %d = %d", sleepyHead.id, mostSleepyMinute, sleepyHead.id*mostSleepyMinute)
}

func parse(data []string, c chan guard) {
	r := regexp.MustCompile(`\[([0-9\- :]+)\] (.+)`)
	guardIDRegex := regexp.MustCompile(`Guard #([0-9]+) begins shift`)
	timeLayout := "2006-01-02 15:04"
	timeFallAsleep := 0
	var currentGuard *guard
	for _, v := range data {
		matches := r.FindStringSubmatch(v)
		time, err := time.Parse(timeLayout, matches[1])
		if err != nil {
			fmt.Println(err.Error())
			return
		}
		stateStr := matches[2]
		switch stateStr {
		case "wakes up":
			currentGuard.addSleepTime(time, timeFallAsleep)
		case "falls asleep":
			timeFallAsleep = time.Minute()
		default:
			if currentGuard != nil {
				c <- *currentGuard
			}
			guardIDMatches := guardIDRegex.FindStringSubmatch(stateStr)
			id, err := strconv.Atoi(guardIDMatches[1])
			if err != nil {
				fmt.Println(err)
				return
			}
			currentGuard = &guard{id, make([]int, 60)}
		}
	}
	c <- *currentGuard
	close(c)
}

func findMostSleepyGuard(guards *map[int]guard) guard {
	var sleepyHead guard
	maxMinutesAsleep := -1
	for _, v := range *guards {
		minutesAsleep := sliceutils.SumInts(v.asleepTimes)
		if minutesAsleep > maxMinutesAsleep {
			maxMinutesAsleep = minutesAsleep
			sleepyHead = v
		}
	}
	return sleepyHead
}

func findMostSleepyMinute(sleepyHead *guard) int {
	maxTimesAsleep := 0
	mostSleepyMinute := -1
	for i, v := range sleepyHead.asleepTimes {
		if v > maxTimesAsleep {
			maxTimesAsleep = v
			mostSleepyMinute = i
		}
	}
	return mostSleepyMinute
}
