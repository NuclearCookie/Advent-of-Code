package main

import (
	"bufio"
	"fmt"
	"strconv"
	"strings"

	"github.com/nuclearcookie/aoc2018/day1/day1-1/input"
)

func main() {
	data := input1.Get()
	scanner := bufio.NewScanner(strings.NewReader(data))

	result := 0
	for scanner.Scan() {
		i, err := strconv.Atoi(scanner.Text())
		if err != nil {
			panic(err)
		}
		result += i
	}
	fmt.Println("Total frequency offset: ", result)
}
