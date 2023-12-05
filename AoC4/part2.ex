defmodule Part2 do
  defp _getAdditionalCards(listOfCards, originalList) do
    Enum.flat_map(listOfCards, fn {id, win} ->
      if win == 0 do
        [{id, win}]
      else
        range = Range.new(id + 1, id + win) # Next card ids
        cards = Enum.filter(originalList, fn {id, _} -> id in range end) # next cards
        Enum.concat([{id, win}], _getAdditionalCards(cards, originalList))
      end
    end)
  end

  def getAdditionalCards(listOfCards \\ []) do
    _getAdditionalCards(listOfCards, listOfCards)
  end # def
end # module
