package main

import (
	"fmt"
	"time"
)

type guard struct {
	id          int
	asleepTimes []int
}

func (receiver guard) addSleepTime(time time.Time, asleepTime int) {
	sleepTimes := receiver.asleepTimes
	awakeTime := time.Minute()
	for i := asleepTime; i < awakeTime; i++ {
		sleepTimes[i]++
	}
}

func (receiver guard) String() string {
	result := fmt.Sprintf("{guard: %d, asleeptimes: ", receiver.id)
	for _, v := range receiver.asleepTimes {
		result += fmt.Sprintf("%d,", v)
	}
	result += "\n"
	return result
}
