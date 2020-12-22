package main

import (
	"fmt"
	"strconv"
	"time"

	"github.com/nuclearcookie/aoc2018/day1/input"
)

func main() {
	start_time := time.Now()
	data := input.GetSplit()
	result := 0
	for _, v := range data {
		i, err := strconv.Atoi(v)
		if err != nil {
			panic(err)
		}
		result += i
	}
	duration := time.Since(start_time)
	fmt.Printf("Duration: %s\n", duration)
	fmt.Println("Total frequency offset: ", result)
}
