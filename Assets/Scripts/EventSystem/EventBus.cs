using UnityEngine;

using System;
using System.Collections.Generic;

/// Marker interface:
/// serve solo per dire "questo oggetto può viaggiare nell'EventBus".
public interface IEventPayload { }

/// Firma standard delle callback:
/// ogni listener riceve un payload generico come IEventPayload.
public delegate void EventSignature(IEventPayload payload);

/// Event bus statico e generico.
/// Ogni tipo T avrà il suo bus separato.
public static class EventBus<T> where T : class, IEventPayload
{
    /// Dizionario:
    /// - chiave = canale evento
    /// - valore = lista di metodi iscritti a quel canale
    private static readonly Dictionary<int, List<EventSignature>> invocationLists =
        new Dictionary<int, List<EventSignature>>();

    /// Converte un nome testuale in un id numerico.
    /// È comodo, ma GetHashCode non è ideale per id persistenti.
    public static int NameToChannel(string channelName) => channelName.GetHashCode();

    /// Versione comoda di Subscribe usando il nome del canale.
    public static void Subscribe(string channelName, EventSignature onDispatch) =>
        Subscribe(NameToChannel(channelName), onDispatch);

    /// Registra un listener su un canale.
    public static void Subscribe(int channel, EventSignature onDispatch)
    {
        // Se il canale non esiste ancora, creo la lista.
        if (!invocationLists.ContainsKey(channel))
            invocationLists[channel] = new List<EventSignature>();

        // Evito di inserire lo stesso listener due volte.
        if (!invocationLists[channel].Contains(onDispatch))
            invocationLists[channel].Add(onDispatch);
    }

    /// Versione comoda di Unsubscribe usando il nome del canale.
    public static void Unsubscribe(string channelName, EventSignature onDispatch) =>
        Unsubscribe(NameToChannel(channelName), onDispatch);

    /// Rimuove un listener dal canale.
    public static void Unsubscribe(int channel, EventSignature onDispatch)
    {
        // Se il canale non esiste, non c'è nulla da fare.
        if (!invocationLists.ContainsKey(channel))
            return;

        invocationLists[channel].Remove(onDispatch);

        // Se non restano listener, rimuovo il canale dal dizionario.
        if (invocationLists[channel].Count == 0)
            invocationLists.Remove(channel);
    }

    /// Versione comoda di Raise usando il nome del canale.
    public static void Raise(string channelName, T payload) =>
        Raise(NameToChannel(channelName), payload);

    /// Invia l'evento a tutti i listener iscritti.
    public static void Raise(int channel, T payload)
    {
        // Se nessuno ascolta quel canale, esco subito.
        if (!invocationLists.TryGetValue(channel, out List<EventSignature> invocationList))
            return;

        // Copia della lista:
        // così posso iterare in sicurezza anche se qualcuno
        // durante Invoke fa Subscribe/Unsubscribe.
        EventSignature[] listeners = invocationList.ToArray();

        // Chiamo tutti i listener passando il payload.
        for (int i = 0; i < listeners.Length; i++)
            listeners[i]?.Invoke(payload);
    }
}
