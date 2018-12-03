package main

import (
	"fmt"
	"time"

	"github.com/nuclearcookie/aoc2018/day2/input"
	"github.com/nuclearcookie/aoc2018/utils/sliceutils"
	"github.com/nuclearcookie/aoc2018/utils/strutils"
)

func main() {
	start_time := time.Now()
	data := input.GetSplit()
	for i1, v1 := range data {
		for i2 := i1 + 1; i2 < len(data); i2++ {
			chars_1 := strutils.SplitChars(v1)
			chars_2 := strutils.SplitChars(data[i2])
			diff := sliceutils.FilterRune(chars_1, func(index int, value rune) bool {
				return value != chars_2[index]
			})

			if len(diff) == 1 {
				index_to_remove := -1
				for i3, v3 := range chars_1 {
					if v3 != chars_2[i3] {
						index_to_remove = i3
						break
					}
				}
				result := append(chars_1[0:index_to_remove], chars_1[index_to_remove+1:]...)
				result_str := string(result)

				duration := time.Since(start_time)
				fmt.Printf("Duration: %s\n", duration)
				fmt.Printf("str1: %s, str2: %s, Common letters between matching boxes: %s\n", v1, data[i2], result_str)
				return
			}
		}
	}
}
