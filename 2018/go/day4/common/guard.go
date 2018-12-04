package common

import (
	"fmt"
	"regexp"
	"sort"
	"strconv"
	"time"

	"github.com/nuclearcookie/aoc2018/utils/sliceutils"
)

type Guard struct {
	ID          int
	asleepTimes []int
}

func (receiver Guard) addSleepTime(time time.Time, asleepTime int) {
	sleepTimes := receiver.asleepTimes
	awakeTime := time.Minute()
	for i := asleepTime; i < awakeTime; i++ {
		sleepTimes[i]++
	}
}

func (receiver Guard) String() string {
	result := fmt.Sprintf("{guard: %d, asleeptimes: ", receiver.ID)
	for _, v := range receiver.asleepTimes {
		result += fmt.Sprintf("%d,", v)
	}
	result += "\n"
	return result
}

func ProcessGuards(data []string) map[int]Guard {
	guards := make(map[int]Guard)
	c := make(chan Guard)
	sort.Strings(data)
	go parse(data, c)
	for newGuard := range c {
		_, exists := guards[newGuard.ID]
		if !exists {
			guards[newGuard.ID] = newGuard
		} else {
			currentGuard := guards[newGuard.ID]
			for i, v := range newGuard.asleepTimes {
				currentGuard.asleepTimes[i] += v
			}
		}
	}
	return guards
}

func parse(data []string, c chan Guard) {
	r := regexp.MustCompile(`\[([0-9\- :]+)\] (.+)`)
	guardIDRegex := regexp.MustCompile(`Guard #([0-9]+) begins shift`)
	timeLayout := "2006-01-02 15:04"
	timeFallAsleep := 0
	var currentGuard *Guard
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
			currentGuard = &Guard{id, make([]int, 60)}
		}
	}
	c <- *currentGuard
	close(c)
}

func FindMostSleepyGuard(guards *map[int]Guard) Guard {
	var sleepyHead Guard
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

func FindMostSleepyMinute(sleepyHead *Guard) int {
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
