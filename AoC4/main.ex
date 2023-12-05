require Part2

input = case File.read("input.txt") do
  {:error, reason} -> IO.puts(reason)
  {:ok, content} -> content
end

get_game_id = fn game -> String.split(game) |> tl |> hd |> String.to_integer(10)  end
string_to_numberlist = fn str -> String.split(str) |> Enum.map(fn number -> String.to_integer(number) end) end

# Part 1
cards = String.split(input, "\n")
  |> Enum.map(fn line -> String.split(line, ":") end)
  |> Enum.map(fn [game, numbers] -> [get_game_id.(game), numbers] end)
  |> Enum.map(fn [id, numbers] -> [id, String.split(numbers, "|")] end)
  |> Enum.map(fn [id, [win, selected]] -> [
    id,
    string_to_numberlist.(win),
    string_to_numberlist.(selected)] end)

winning_numbers_per_card = Enum.map(cards, fn [_, win, selected] -> [win, selected] end)
  |> Enum.map(fn [win, selected] -> [Enum.dedup(win), Enum.dedup(selected)] end)
  |> Enum.map(fn [win, selected] -> Enum.concat(win, selected) end)
  |> Enum.map(&Enum.sort/1)
  |> Enum.map(&Enum.frequencies/1)
  |> Enum.map(fn pair -> (Map.filter(pair, fn {_key, val} -> val == 2 end)) end)

points = Enum.filter(winning_numbers_per_card, fn map -> map != %{} end)
  |> Enum.map(fn pair -> (Enum.reduce(pair, 1, fn {_key, val}, acc -> acc * val end)) end)
  |> Enum.map(fn num -> Integer.floor_div(num, 2) end)
  |> Enum.sum()

IO.puts(points)

# Part 2
# Knowing how many numbers were good in a card (and its ID), I can add them to a list
winning_numbers_with_id = Enum.zip(Enum.map(cards, fn [id, _, _] -> id end), winning_numbers_per_card)
wins_for_a_card = Enum.map(
  winning_numbers_with_id,
  fn {id, numbers} -> {id, Enum.reduce(numbers, 0, fn {_, _}, acc -> acc + 1 end)} end)

Part2.getAdditionalCards(wins_for_a_card)
  |> Enum.reduce(0, fn _, acc -> acc + 1 end)
  |> IO.inspect()
