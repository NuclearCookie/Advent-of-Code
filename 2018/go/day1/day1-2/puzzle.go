package main

import (
	"fmt"
	"strconv"
	"time"

	"github.com/nuclearcookie/aoc2018/day1/input"
)

var frequency_map map[int]bool

func main() {
	start_time := time.Now()
	frequency_map = make(map[int]bool)
	data := input.GetSplit()
	frequency := 0

	loop(data, frequency)
	duration := time.Since(start_time)
	fmt.Printf("Duration: %s\n", duration)
}

func loop(data []string, frequency int) {
	for _, v := range data {
		i, err := strconv.Atoi(v)
		if err != nil {
			panic(err)
		}
		frequency += i
		if frequency_map[frequency] {
			fmt.Println("First duplicate frequency: ", frequency)
			return
		} else {
			frequency_map[frequency] = true
		}
	}
	loop(data, frequency)
}
