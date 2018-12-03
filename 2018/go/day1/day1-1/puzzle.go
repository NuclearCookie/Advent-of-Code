package main

import (
	"fmt"
	"strconv"

	"github.com/nuclearcookie/aoc2018/day1/input"
)

func main() {
	data := input.GetSplit()
	result := 0
	for _, v := range data {
		i, err := strconv.Atoi(v)
		if err != nil {
			panic(err)
		}
		result += i
	}
	fmt.Println("Total frequency offset: ", result)
}
