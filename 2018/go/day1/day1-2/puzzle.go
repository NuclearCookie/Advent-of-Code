package main

import (
	"bufio"
	"fmt"
	"strconv"
	"strings"

	"github.com/nuclearcookie/aoc2018/day1/input"
)

var frequency_map map[int]bool

func main() {
	frequency_map = make(map[int]bool)
	data := input1.Get()
	frequency := 0

	loop(data, frequency)
}

func loop(data string, frequency int) {
	scanner := bufio.NewScanner(strings.NewReader(data))
	for scanner.Scan() {
		i, err := strconv.Atoi(scanner.Text())
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
