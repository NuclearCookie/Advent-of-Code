package main

import (
	"fmt"
	"regexp"
	"sort"
	"strconv"
	"time"

	"github.com/nuclearcookie/aoc2018/day4/input"
)

func main() {
	startTime := time.Now()
	data := input.GetSplit()
	slice := eventSlice{make([]timeEvent, len(data))}
	c := make(chan timeEvent, len(data))
	go parse(data, c)
	i := 0
	for event := range c {
		slice.sortedEvents[i] = event
		i++
	}
	sort.Sort(&slice)
	fmt.Printf("%+v\n", slice.sortedEvents)
	processGuardData(&slice)

	duration := time.Since(startTime)
	fmt.Printf("Duration: %s\n", duration)
}

func parse(data []string, c chan timeEvent) {
	r := regexp.MustCompile(`\[([0-9\- :]+)\] (.+)`)
	guardIDRegex := regexp.MustCompile(`Guard #([0-9]+) begins shift`)
	timeLayout := "2006-01-02 15:04"
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
			c <- toggleState{time, false}
		case "falls asleep":
			c <- toggleState{time, true}
		default:
			guardIDMatches := guardIDRegex.FindStringSubmatch(stateStr)
			id, err := strconv.Atoi(guardIDMatches[1])
			if err != nil {
				fmt.Println(err)
				return
			}
			c <- changeGuard{time, id}
		}
	}
	close(c)
}

func processGuardData(slice *eventSlice) {
	for _, v := range slice.sortedEvents {
		switch t := v.(type) {
		case toggleState:
		case changeGuard:

		}
	}
}
