import AoCGame

{reds, greens, blues} = {12, 13, 14}

input = case File.read("input.txt") do
  {:error, reason} -> IO.puts(reason)
  {:ok, content} -> content
end

get_game_id = fn input -> String.split(input) |> tl |> hd |> String.to_integer(10) end
split_single_games = fn game -> String.split(game, ",") end

grab_key = fn grab -> String.split(grab) |> tl |> hd end
grab_value = fn grab -> String.split(grab) |> hd |> String.to_integer(10) end

get_max_value_from_kv_pair = fn kvpair -> Enum.reduce(kvpair, %{}, fn {key, values}, acc ->
  max_value = Enum.max(values)
  Map.put(acc, key, max_value)
end) end

String.split(input, "\n")
  |> Enum.map(fn line -> String.split(line, ":") end)
  |> Enum.map(fn [game, plays] -> [get_game_id.(game), String.split(plays, ";")] end)
  |> Enum.map(fn [id, plays_list] -> [id, Enum.flat_map(plays_list, split_single_games)] end)
  |> Enum.map(fn [id, plays] -> [ id, Enum.group_by(plays, grab_key, grab_value) ] end)
  |> Enum.map(fn [id, plays] -> [id, get_max_value_from_kv_pair.(plays)] end)
  |> Enum.map(fn [id, plays] -> %AoCGame{id: id, red: plays["red"], green: plays["green"], blue: plays["blue"]} end)
  |> Enum.filter(fn game -> game.red <= reds and game.green <= greens and game.blue <= blues end)
  |> Enum.map(fn game -> game.id end)
  |> Enum.sum()
  |> IO.inspect()
