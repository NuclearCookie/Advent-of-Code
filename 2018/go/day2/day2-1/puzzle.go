package main

import (
	"bufio"
	"fmt"
	"strings"

	"github.com/nuclearcookie/aoc2018/day2/input"
)

var character_occurence map[rune]int

func main() {
	data := input1.Get()
	scanner := bufio.NewScanner(strings.NewReader(data))
	boxes_with_2_same_runes := 0
	boxes_with_3_same_runes := 0

	for scanner.Scan() {
		character_occurence = make(map[rune]int)
		box_id := scanner.Text()
		chars := []rune(box_id)
		for _, v := range chars {
			character_occurence[v] += 1
		}
		contains_2 := false
		contains_3 := false
		for _, v := range character_occurence {
			if v == 2 {
				contains_2 = true
			}
			if v == 3 {
				contains_3 = true
			}
		}

		if contains_2 {
			boxes_with_2_same_runes++
		}
		if contains_3 {
			boxes_with_3_same_runes++
		}
	}

	fmt.Println("Checksum: ", boxes_with_2_same_runes*boxes_with_3_same_runes)
}
