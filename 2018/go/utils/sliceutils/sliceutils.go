package sliceutils

type StringsObj struct{}
type RunesObj struct{}
type IntsObj struct{}

var Strings StringsObj
var Runes RunesObj
var Ints IntsObj

func (strings StringsObj) Filter(a []string, f func(int, string) bool) []string {
	vsf := make([]string, 0)
	for i, v := range a {
		if f(i, v) {
			vsf = append(vsf, v)
		}
	}
	return vsf
}

func (runes RunesObj) Filter(a []rune, f func(int, rune) bool) []rune {
	vsf := make([]rune, 0)
	for i, v := range a {
		if f(i, v) {
			vsf = append(vsf, v)
		}
	}
	return vsf
}

func (ints IntsObj) Sum(a []int) int {
	count := 0
	for _, v := range a {
		count += v
	}
	return count
}

func (ints IntsObj) Remove(slice []int, index int) []int {
	sliceLen := len(slice)
	if index == sliceLen-1 {
		slice = slice[0:index]
	} else if len(slice) > 0 {
		slice = append(slice[0:index], slice[index+1:]...)
	}
	return slice
}

func (ints IntsObj) AppendIfMissing(slice []int, i int) []int {
	for _, ele := range slice {
		if ele == i {
			return slice
		}
	}
	return append(slice, i)
}
