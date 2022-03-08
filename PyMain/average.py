class average:
    def __init__(self) -> None:
        self.sum = 0.0
        self.count = 0

    def add(self, value: float) -> None:
        self.sum += value
        self.count += 1

    @property
    def value(self) -> float:
        return 0.0 if self.count == 0 else self.sum / self.count
