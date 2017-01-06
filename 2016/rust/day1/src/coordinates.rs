pub struct Coordinates {
    pub horizontal_movement : i32,
    pub vertical_movement : i32
}

impl Coordinates {
    pub fn new() -> Coordinates {
        Coordinates { horizontal_movement : 0, vertical_movement : 0 }
    }
}
